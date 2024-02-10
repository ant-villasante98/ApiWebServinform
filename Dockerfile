FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

COPY . .
RUN dotnet restore


RUN dotnet publish ./Servirform.csproj -o /out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /out .
CMD ["dotnet","Servirform.dll"]
