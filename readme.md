# swagger ui center

## redis server (for auth)

```bash
docker run -d -p 6379:6379 --name local-redis redis
```

## key clock prepare

### run key-clock

```bash
docker run -d -p 8080:8080 -p 8443:8443 \
    -e KEYCLOAK_ADMIN=user \
    -e KEYCLOAK_ADMIN_PASSWORD=password \
    quay.io/keycloak/keycloak start-dev
```

### import test client

client id testing json : [link](./keyclock/test_client.json)

- open clients page
    ![keyclock-clients-page.png](./img/keyclock-clients-page.png)
- click import client
    ![keyclock-clients-import-button.png](./img/keyclock-clients-import-button.png)

## memo

- [Swashbuckle.AspNetCore is being removed in .NET 9](https://github.com/dotnet/aspnetcore/issues/54599)