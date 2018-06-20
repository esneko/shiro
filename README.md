# shiro

REST API powered by [.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)

## Run locally

```
$ dotnet restore
$ dotnet run
```

## Run container

```
$ docker build --pull -t shiro .
$ docker run --rm -it -p 80:80 --name shiro-app shiro
```

## API documentation

Swagger dynamically generate [API documentation](http://localhost:80/swagger)

### Create user
```
curl --request POST \
  --url http://localhost:80/api/user \
  --header 'content-type: application/json' \
  --data '{"username": "esneko", "password": "qwerty"}'
```

### Request token
```
curl --request POST \
  --url http://localhost:80/api/token \
  --header 'content-type: application/json' \
  --data '{"username": "esneko", "password": "qwerty"}'
```

### Authenticate requests
```
curl --request GET \
  --url http://localhost:80/api/user \
  --header 'authorization: Bearer __REPLACE_WITH_TOKEN__'
```