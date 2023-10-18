#region File Information
//===============================================================================
// Author:  SEBASTIAN FERNANDEZ VERGARA.
// Date:    2023-04-26
// Comment: Authentication control for the auditor user
//===============================================================================
#endregion

using MediatR;
using Oxxo.Cloud.Logging.Domain.Enums;
using Oxxo.Cloud.Security.Application.Common.DTOs;
using Oxxo.Cloud.Security.Application.Common.Exceptions;
using Oxxo.Cloud.Security.Application.Common.Helper;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Models;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.Net;


namespace Oxxo.Cloud.Security.Application.InvFis.Command.AuthInvfis;

public class AuthInvfisCommand : BaseProperties, IRequest<GenResponse<bool>>
{
    [Required]
    public int Store_Place_Id { get; set; }
    [Required]
    public string CrStore { get; set; } = string.Empty;
    [Required]
    public int Auditor { get; set; }
    public string Password { get; set; } = string.Empty;
    public string InventoryType { get; set; } = string.Empty;
    [Required]
    public string CurrentStoreDate { get; set; } = string.Empty;
    public string TokenAuth { get; set; } = string.Empty;
    [Required]
    public bool ActivatedCore { get; set; }

    public class AuthInvfisCommandHandler : IRequestHandler<AuthInvfisCommand, GenResponse<bool>>
    {
        private readonly ILogService logService;
        private readonly IExternalService externalService;
        /// <summary>
        /// Constructor that injects the database context.
        /// </summary>
        /// <param name="context"></param>
        public AuthInvfisCommandHandler(ILogService logService, IExternalService externalService)
        {
            this.logService = logService;
            this.externalService = externalService;
        }

        /// <summary>
        /// this function is responsible for performing request to retrive specific data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <exception cref="NotFoundException"></exception>	
        public async Task<GenResponse<bool>> Handle(AuthInvfisCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TokenAuth))
                {
                    return new GenResponse<bool>(HttpStatusCode.Unauthorized, false);
                }

                HashInvFisDTO hashInvFisDTO = await GetHastInvFisData(request);
                return ValidateAlgAccess(hashInvFisDTO);

            }
            catch (Exception ex)
            {
                await logService.Logger(GlobalConstantHelpers.LOG_METHOD_LOGIN_INVFIS, GlobalConstantHelpers.METHODCOMMANDHANDLER, LogTypeEnum.Error,
                   request.UserIdentification, $"{GlobalConstantMessages.LOGIN_INVFIS_ERROR_API_POST} {ex.GetException()}", GlobalConstantHelpers.NAMECLASSLOGIN_INVFISINVQUERY);
                return new GenResponse<bool>(HttpStatusCode.BadRequest, false);
            }
        }

        private async Task<HashInvFisDTO> GetHastInvFisData(AuthInvfisCommand request)
        {
            GenResponse<List<HistoryInvResponse>> dataRequest;
            HashInvFisDTO hashInvFisDTO = new();
            if (request.ActivatedCore)
            {
                var endpoint = Environment.GetEnvironmentVariable(GlobalConstantHelpers.URL_PHYINVENTORY_HISTORY);
                string uri = !string.IsNullOrEmpty(endpoint) ? string.Format("{0}?storePlaceId={1}&top={2}", endpoint, request.Store_Place_Id, 2) : string.Empty;
                dataRequest = await this.externalService.CallToExternalApiAsync<List<HistoryInvResponse>>(uri, request.TokenAuth, RestMethodEnum.GET);
            }
            else
            {
                dataRequest = new GenResponse<List<HistoryInvResponse>>(HttpStatusCode.OK, new List<HistoryInvResponse>());
            }

            if (dataRequest.StatusCode == HttpStatusCode.OK && dataRequest.Body != null)
            {
                List<HistoryInvResponse> historyInventoryList;
                historyInventoryList = dataRequest.Body;
                if (historyInventoryList != null)
                {
                    
                    if (request.ActivatedCore)
                    {
                        _ = historyInventoryList.OrderByDescending(x => x.LOGBOOK_ID).ToList();
                        HistoryInvResponse currentInv = historyInventoryList.First();
                        HistoryInvResponse previusInv = historyInventoryList.Last();

                        bool hasPrevius = historyInventoryList.Count > 1;
                        InvType PrevInventoryType = GetInventoryType(previusInv);
                        bool PrevInventorySuccess = GetPreviousSuccess(previusInv);
                        DateTime prevInvDate = hasPrevius ? previusInv.BUSINESS_DATE : DateTime.Parse(request.CurrentStoreDate);

                        hashInvFisDTO = new()
                        {
                            CurrentStoreDate = DateTime.Parse(request.CurrentStoreDate),
                            AdminDate = currentInv.BUSINESS_DATE,
                            Auditor = request.Auditor,
                            CrStore = request.CrStore,
                            InvType = request.InventoryType,
                            IsPreviusInvSuccess = PrevInventorySuccess,
                            PreviusAdminDate = prevInvDate,
                            PreviusInvType = PrevInventoryType,
                            Password = request.Password,
                            HasPrevius = hasPrevius,
                            ActivatedCore = request.ActivatedCore,
                        };
                    }
                    else
                    {
                        hashInvFisDTO = new()
                        {
                            CurrentStoreDate = DateTime.Parse(request.CurrentStoreDate),
                            Auditor = request.Auditor,
                            CrStore = request.CrStore,
                            Password = request.Password,
                            ActivatedCore = request.ActivatedCore,
                        };
                    }
                }
            }

            return hashInvFisDTO;

        }

        private static InvType GetInventoryType(HistoryInvResponse previousInventoryResponse)
        {
            InvType inventoryType = InvType.PHYSIC;
            if (previousInventoryResponse.INVENTORY_TYPE_ID != null)
            {
                int inventoryTypeId = int.Parse(previousInventoryResponse.INVENTORY_TYPE_ID);

                inventoryType = inventoryTypeId switch
                {
                    (int)InvType.CICLIC => InvType.CICLIC,
                    (int)InvType.PHYSIC => InvType.PHYSIC,
                    _ => InvType.STEALING,
                };
            }


            return inventoryType;
        }

        private static bool GetPreviousSuccess(HistoryInvResponse previousInventoryResponse)
        {
            bool isSuccess = false;

            if (previousInventoryResponse.INVENTORY_STATUS_ID != null && previousInventoryResponse.ADVANCE_STATUS_ID != null)
            {

                bool inventoryStatusParsed = int.TryParse(previousInventoryResponse.INVENTORY_STATUS_ID, out int inventoryStatusId);
                bool advanceStatusParsed = int.TryParse(previousInventoryResponse.ADVANCE_STATUS_ID, out int advanceStatusId);

                if (inventoryStatusParsed && advanceStatusParsed)
                {
                    if (inventoryStatusId == (int)InvStatusType.CLOSED && advanceStatusId == (int)InvAdvanceType.CLOSED)
                    {
                        isSuccess = true;
                    }
                }
            }

            return isSuccess;
        }

        private static GenResponse<bool> ValidateAlgAccess(HashInvFisDTO hashInvFisDTO)
        {
            var properties = hashInvFisDTO.GetType().GetProperties();
            int totalResult = 0;
            Dictionary<string, int> keys = new();
            foreach (var prop in properties)
            {
                string? dataValue = Convert.ToString(prop.GetValue(hashInvFisDTO));
                if (!string.IsNullOrEmpty(dataValue))
                {
                    keys.Add(prop.Name, GetConversionByType(prop.Name, dataValue, hashInvFisDTO));
                    totalResult += keys[prop.Name];
                }
            }
            if (GetVerificatorDigit(totalResult, hashInvFisDTO.Password))
            {
                return new GenResponse<bool>(HttpStatusCode.OK, true);
            }
            else
            {
                return new GenResponse<bool>(HttpStatusCode.Unauthorized, false);
            }
        }

        private static int GetConversionByType(string propertyName, string dataValue, HashInvFisDTO hashInvFisDTO)
        {
            int result;
            switch (propertyName)
            {
                case "CrStore":
                    int code1 = GetConversion(dataValue[..1]) * 10000;
                    int code2 = GetConversion(dataValue.Substring(1, 1)) * 100;
                    int code3 = GetConversion(dataValue.Substring(2, 1));
                    result = code1 + code2 + code3;
                    break;

                case "Auditor":
                    result = int.Parse(dataValue);
                    break;

                case "CurrentStoreDate":
                    {
                        DateTime dateToTake;
                        if (hashInvFisDTO.ActivatedCore)
                        {

                            if (hashInvFisDTO.HasPrevius)
                            {
                                if (hashInvFisDTO.PreviusInvType == InvType.CICLIC && !hashInvFisDTO.IsPreviusInvSuccess)
                                {
                                    dateToTake = hashInvFisDTO.PreviusAdminDate;
                                }
                                else
                                {
                                    dateToTake = hashInvFisDTO.AdminDate;
                                }
                            }
                            else
                            {
                                dateToTake = !string.IsNullOrEmpty(hashInvFisDTO.AdminDate.ToString("dd/MM/yyyy"))
                                    ? hashInvFisDTO.AdminDate
                                    : hashInvFisDTO.CurrentStoreDate;
                            }
                        }
                        else
                        {
                            dateToTake = hashInvFisDTO.CurrentStoreDate;
                        }


                        int dayOfYear = GetDayOfYear(dateToTake);
                        if (dayOfYear % 2 == 0)
                        {
                            result = dayOfYear;
                        }
                        else
                        {
                            result = dayOfYear * 100;
                        }

                        int yearDate = dateToTake.Year;
                        result += yearDate;
                    }
                    break;

                case "InvType":
                    {
                        if (hashInvFisDTO.ActivatedCore)
                        {
                            if (hashInvFisDTO.HasPrevius && hashInvFisDTO.PreviusInvType == InvType.CICLIC && !hashInvFisDTO.IsPreviusInvSuccess)
                            {
                                hashInvFisDTO.InvType = "C";
                            }
                            result = GetConversion(hashInvFisDTO.InvType);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    break;

                default:
                    result = 0;
                    break;
            }

            return result;

        }

        private static int GetConversion(string dataValue)
        {
            char chValue = char.Parse(dataValue);
            return Convert.ToInt32(chValue);
        }

        private static int GetDayOfYear(DateTime date)
        {
            var days = (date - new DateTime(date.Year, 1, 1)).Days + 1;
            return int.Parse(days.ToString("000"));
        }

        private static bool GetVerificatorDigit(int totalResult, string password)
        {
            int digit = 0;
            foreach (char c in totalResult.ToString())
            {
                digit += int.Parse(c.ToString());
            }
            string resultHash = string.Format("{0}{1}", totalResult.ToString("D13")[5..], digit);

            return password == resultHash;
        }
    }
}