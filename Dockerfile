FROM mcr.microsoft.com/dotnet/sdk:6.0 as build

ARG PROJFILE=*.csproj
ARG MAIN_PROJECT_NAME=CompanyEmployees
WORKDIR /home/app

COPY ./*.sln .
COPY ../*/$PROJFILE .

RUN for file in $(ls $PROJFILE); do mkdir -p ./${file%.*}/ &&  mv $file ./${file%.*}/; done
RUN dotnet restore ./${MAIN_PROJECT_NAME}/${MAIN_PROJECT_NAME}.csproj
COPY . .
RUN dotnet publish ./${MAIN_PROJECT_NAME}/${MAIN_PROJECT_NAME}.csproj -o /publish/ --no-restore
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /publish
COPY --from=build /publish .
ENV ASPNETCORE_URLS=https://+:5001;http://+:5000
ENTRYPOINT ["dotnet" , "CompanyEmployees.dll" ]
