DOTNET_IMAGE := mcr.microsoft.com/dotnet/sdk:10.0
NUGET_VOLUME := dotnet-nuget

.PHONY: dotnet nuget-init

nuget-init:
	podman volume create $(NUGET_VOLUME) || true

dotnet: nuget-init
	podman run --rm -it \
		-v "$(CURDIR):/workspace:Z" \
		-v $(NUGET_VOLUME):/root/.nuget/packages \
		-w /workspace \
		$(DOTNET_IMAGE) \
		dotnet $(ARGS)

dev-shell: nuget-init
	podman run --rm -it \
		-v "$(CURDIR):/workspace:Z" \
		-v $(NUGET_VOLUME):/root/.nuget/packages \
		-w /workspace \
		$(DOTNET_IMAGE) \
