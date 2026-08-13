# Requires GNU Make + PowerShell 7 (pwsh) on PATH.
SHELL := pwsh.exe
.SHELLFLAGS := -NoProfile -Command

BACKEND_DIR := src/backend/WorkflowPlatform.Api
SOLUTION_DIR := src/backend
FRONTEND_DIR := src/frontend

.PHONY: help up up-backend up-frontend test test-backend test-frontend

.DEFAULT_GOAL := help

help:
	@echo "Available targets:"
	@echo "  make up            - start backend and frontend, each in its own window"
	@echo "  make up-backend    - start only the backend API   (http://localhost:5175)"
	@echo "  make up-frontend   - start only the frontend       (http://localhost:4200)"
	@echo "  make test          - run backend and frontend test suites"
	@echo "  make test-backend  - run only the backend test suite"
	@echo "  make test-frontend - run only the frontend test suite"

up: up-backend up-frontend
	@echo "Backend starting:  http://localhost:5175"
	@echo "Frontend starting: http://localhost:4200"

up-backend:
	@Start-Process pwsh -ArgumentList '-NoExit','-Command','dotnet run' -WorkingDirectory '$(BACKEND_DIR)'

up-frontend:
	@Start-Process pwsh -ArgumentList '-NoExit','-Command','npm start' -WorkingDirectory '$(FRONTEND_DIR)'

test: test-backend test-frontend

test-backend:
	@Set-Location '$(SOLUTION_DIR)'; dotnet test

test-frontend:
	@Set-Location '$(FRONTEND_DIR)'; npm test -- --watch=false
