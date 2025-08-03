FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SelfServicePortalWithIdentity.dll"]


# Instructions to build and push the image to OpenShift registry:
#
# 1. Log in to the OpenShift cluster and switch to your project:
#    oc login https://<cluster-url>:6443 -u <user> -p <password>
#    oc project <your-project>
#
# 2. Get the internal registry route:
#    REGISTRY=$(oc get route default-route -n openshift-image-registry --template='{{ .spec.host }}')
#
#    PROJECT=$(oc project -q)
#    IMAGE=$REGISTRY/$PROJECT/selfservice-portal:latest
#
# 3. Log in to the registry:
#    TOKEN=$(oc whoami -t)
#    podman login --tls-verify=false -u openshift -p $TOKEN $REGISTRY
#
#
# 4. Build and push the image:
#    podman build -t $IMAGE .
#    podman push --tls-verify=false $IMAGE

#
# 5. Update openshift/deployment.yaml to use the image $IMAGE and apply it:
#    oc apply -f openshift/deployment.yaml
#
# 6. Verify and access:
#    oc get pods
#    oc get svc
#    oc get route

