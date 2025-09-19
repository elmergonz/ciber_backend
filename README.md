# ciber_backend

## build instructions (develpment)

### docker
```sh
# Build image
docker build -t ciber-backend .

# Run container
docker run -d -p 5005:8080 --name ciber-backend ciber-backend
```

### dotnet
```sh
# Run inside root folder
dotnet run
```