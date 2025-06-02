#!/bin/bash

# === Настройки (можно изменить под себя) ===
RESOURCE_GROUP="desk-booking-rg"
LOCATION="westeurope"
PLAN_NAME="desk-booking-plan"
APP_NAME="desk-booking-app-test"
DOCKER_COMPOSE_FILE="docker-compose.yml"

# === Начало ===
echo "=== 🚀 Azure Deploy Script for Desk Booking App ==="

# === Вход в Azure ===
echo "[1/6] 🔐 Вход в Azure..."
az login || { echo "❌ Не удалось выполнить вход в Azure."; exit 1; }

# === Создание ресурсной группы ===
echo "[2/6] 📁 Создание ресурсной группы '$RESOURCE_GROUP'..."
az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

# === Создание App Service Plan ===
echo "[3/6] ⚙️  Создание App Service Plan '$PLAN_NAME'..."
az appservice plan create --name "$PLAN_NAME" --resource-group "$RESOURCE_GROUP" --is-linux

# === Создание Web App ===
echo "[4/6] 🌐 Создание Web App '$APP_NAME'..."
az webapp create \
  --resource-group "$RESOURCE_GROUP" \
  --plan "$PLAN_NAME" \
  --name "$APP_NAME" \
  --multicontainer-config-type compose \
  --multicontainer-config-file "$DOCKER_COMPOSE_FILE"

# === Переменные окружения из .env (если есть) ===
if [ -f .env ]; then
  echo "[5/6] 📦 Загрузка переменных окружения из .env..."
  az webapp config appsettings set --resource-group "$RESOURCE_GROUP" --name "$APP_NAME" --settings $(cat .env | xargs)
else
  echo "[5/6] ⚠️  Файл .env не найден. Переменные окружения не установлены."
fi

# === Открытие сайта ===
echo "[6/6] 🌍 Открытие сайта..."
echo "Откройте сайт вручную в браузере:"
echo "➡️  https://$APP_NAME.azurewebsites.net"
az webapp browse --resource-group "$RESOURCE_GROUP" --name "$APP_NAME"

# === Готово ===
echo -e "\n✅ Деплой завершён!"
echo "🔗 URL: https://$APP_NAME.azurewebsites.net"
