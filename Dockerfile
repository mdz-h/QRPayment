FROM registry.access.redhat.com/ubi8/dotnet-60:6.0
ARG XPOS_PROJECT
WORKDIR /opt/app-root/app
COPY --chown=1001 . .
RUN mkdir ./bin
RUN tar -xf app.tar.gz -C /opt/app-root/app/bin
WORKDIR /opt/app-root/app/bin

EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000
CMD ["sh","-c","dotnet Oxxo.Cloud.Security.WebUI.dll"]
