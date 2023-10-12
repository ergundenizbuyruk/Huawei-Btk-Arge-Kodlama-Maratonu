#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Huawei.Btk.MVC/Huawei.Btk.MVC.csproj", "Huawei.Btk.MVC/"]
COPY ["Huawei.Btk.Application/Huawei.Btk.Application.csproj", "Huawei.Btk.Application/"]
COPY ["Huawei.Btk.Core/Huawei.Btk.Core.csproj", "Huawei.Btk.Core/"]
RUN dotnet restore "Huawei.Btk.MVC/Huawei.Btk.MVC.csproj"
COPY . .
WORKDIR "/src/Huawei.Btk.MVC"
RUN dotnet build "Huawei.Btk.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Huawei.Btk.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Huawei.Btk.MVC.dll"]