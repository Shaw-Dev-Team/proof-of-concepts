# Requires GNU Make + PowerShell 7 (pwsh) on PATH.
SHELL := pwsh.exe
.SHELLFLAGS := -NoProfile -Command

BACKEND_DIR := src/backend/WorkflowPlatform.Api
FRONTEND_DIR := src/frontend

.PHONY: help up up-backend up-frontend

.DEFAULT_GOAL := help

help:
	@echo "Available targets:"
	@echo "  make up            - start backend and frontend, each in its own window"
	@echo "  make up-backend    - start only the backend API   (http://localhost:5175)"
	@echo "  make up-frontend   - start only the frontend       (http://localhost:4200)"

up: up-backend up-frontend
	@echo "Backend starting:  http://localhost:5175"
	@echo "Frontend starting: http://localhost:4200"

up-backend:
	@Start-Process pwsh -ArgumentList '-NoExit','-Command','dotnet run' -WorkingDirectory '$(BACKEND_DIR)'

up-frontend:
	@Start-Process pwsh -ArgumentList '-NoExit','-Command','npm start' -WorkingDirectory '$(FRONTEND_DIR)'
