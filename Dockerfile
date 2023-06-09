#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY electronicassistantwebapi/*.csproj .
RUN dotnet restore --use-current-runtime  

# copy everything else and build app
COPY electronicassistantwebapi/. .
RUN dotnet publish --use-current-runtime --self-contained false --no-restore -o /app


# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "electronicassistantwebapi.dll"]

