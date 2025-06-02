#!/bin/bash
# Скрипт для автоматического деплоя Desk Booking Application на Azure Web App for Containers

# === Настройки (замените на свои значения при необходимости) ===
RESOURCE_GROUP="desk-booking-rg"
LOCATION="westeurope"
PLAN_NAME="desk-booking-plan"
APP_NAME="desk-booking-app-$(date +%s)"
DOCKER_COMPOSE_FILE="docker-compose.yml"

# === Вход в Azure ===
echo "[1/6] Вход в Azure..."
az login

# === Создание ресурсной группы ===
echo "[2/6] Создание ресурсной группы $RESOURCE_GROUP..."
az group create --name $RESOURCE_GROUP --location $LOCATION

# === Создание App Service Plan ===
echo "[3/6] Создание App Service Plan $PLAN_NAME..."
az appservice plan create --name $PLAN_NAME --resource-group $RESOURCE_GROUP --is-linux

# === Создание Web App с Docker Compose ===
echo "[4/6] Создание Web App $APP_NAME..."
az webapp create \
  --resource-group $RESOURCE_GROUP \
  --plan $PLAN_NAME \
  --name $APP_NAME \
  --multicontainer-config-type compose \
  --multicontainer-config-file $DOCKER_COMPOSE_FILE

# === (Опционально) Настройка переменных окружения из .env ===
if [ -f .env ]; then
  echo "[5/6] Настройка переменных окружения из .env..."
  az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings $(cat .env | xargs)
else
  echo "[5/6] Файл .env не найден, пропускаю настройку переменных окружения."
fi

# === Открыть сайт ===
echo "[6/6] Открытие сайта..."
az webapp browse --resource-group $RESOURCE_GROUP --name $APP_NAME

echo "\nГотово! Ваше приложение деплоится на Azure Web App: https://$APP_NAME.azurewebsites.net" 