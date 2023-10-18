using System.Xml.Serialization;

namespace Oxxo.Cloud.Security.Application.Common.DTOs
{
    public class ResponseExternalCommDto
    {
        public ResponseExternalCommDto()
        {
            IsValid = false;
            StatusCode = 0;
            Data = null;
            Message = string.Empty;
        }

        public bool IsValid { get; set; }
        public int StatusCode { get; set; }
        public dynamic? Data { get; set; }
        public string Message { get; set; }
    }

    [XmlRoot(ElementName = "header")]
    public class Header
    {

        [XmlAttribute(AttributeName = "application")]
        public string Application { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "entity")]
        public string Entity { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "operation")]
        public string Operation { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "source")]
        public string Source { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "folio")]
        public Int64 Folio { get; set; }

        [XmlAttribute(AttributeName = "plaza")]
        public string Plaza { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "tienda")]
        public string Tienda { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "caja")]
        public int Caja { get; set; }

        [XmlAttribute(AttributeName = "adDate")]
        public int AdDate { get; set; }

        [XmlAttribute(AttributeName = "pvDate")]
        public double PvDate { get; set; }
    }

    [XmlRoot(ElementName = "validpass")]
    public class Validpass
    {

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "idusuario")]
        public string Idusuario { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "rol")]
        public string Rol { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "isvalidpasswd")]
        public string Isvalidpasswd { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "isvalidstore")]
        public string Isvalidstore { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "isCreated")]
        public string IsCreated { get; set; } = string.Empty;

        [XmlAttribute(AttributeName = "timevalid")]
        public int Timevalid { get; set; }
    }

    [XmlRoot(ElementName = "wmCode")]
    public class WmCode
    {

        [XmlAttribute(AttributeName = "value")]
        public int Value { get; set; }

        [XmlAttribute(AttributeName = "desc")]
        public string Desc { get; set; } = string.Empty;
    }

    [XmlRoot(ElementName = "response")]
    public class Response
    {
        public Response()
        {
            Validpass = new Validpass();
            WmCode = new WmCode();
        }

        [XmlElement(ElementName = "validpass")]
        public Validpass Validpass { get; set; }

        [XmlElement(ElementName = "wmCode")]
        public WmCode WmCode { get; set; }
    }

    [XmlRoot(ElementName = "TPEDoc")]
    public class TpeDoc
    {
        public TpeDoc()
        {
            Response = new Response();
        }
        [XmlElement(ElementName = "header")]
        public Header? Header { get; set; }

        [XmlElement(ElementName = "response")]
        public Response Response { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public double Version { get; set; }
    }
}


