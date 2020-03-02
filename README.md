# k8s-client

### .NET Core API com [KubernetesClient](https://github.com/kubernetes-client/csharp) package

## Estrutura

- A API conta com as seguintes controllers: 
- Namespaces: Lista, cria a remove namespaces
- Services: Lista, cria e remove serviços
- Pods: Lista, cria e remove pods
- Jobs: Lista, cria e remove jobs

### Namespaces
- [GET] (api_url/namespaces) - Lista todos os namespaces do Kubernetes
- [GET] (api_url/namespaces/{uid}) - Lista o namespace que contém o uid fornecido
- [GET] (api_url/namespaces/{name}/pods) - Lista os pods do namespace que contém o nome fornecido
- [GET] (api_url/namespaces/{name}/services) - Lista os services do namespace que contém o nome fornecido
- [GET] (api_url/namespaces/{name}/jobs) - Lista os jobs do namespace que contém o nome fornecido
- [POST] (api_url/namespaces?name={name}) - Cria um namespace com o nome fornecido
- [DELETE] (api_url/namespaces?name={name}) - Remove o namespace com nome fornecido

### Services
- [GET] (api_url/services) - Lista todos os services de todos os namespaces do Kubernetes
- [GET] (api_url/services/{uid}) - Lista o serviço que contém o uid fornecido
- [DELETE] (api_url/services?name={name}&namespaceName={nsName}) - Remove o serviço do namespace com nome fornecido

### Pods
- [GET] (api_url/pods) - Lista todos os pods de todos os namespaces do Kubernetes
- [GET] (api_url/pods/{uid}) - Lista o pod que contém o uid fornecido
- [POST] (api_url/pods?name={name}&namespaceName={nsName}) - Cria um pod com o nome fornecido no namespace com o nome fornecido
- [DELETE] (api_url/pods?name={name}&namespaceName={nsName}) - Remove o pod do namespace com nome fornecido

### Jobs
- [GET] (api_url/jobs) - Lista todos os jobs de todos os namespaces do Kubernetes
- [GET] (api_url/jobs/{uid}) - Lista o job que contém o uid fornecido
- [POST] (api_url/jobs?name={name}&namespaceName={nsName}) - Cria um job com o nome fornecido no namespace com o nome fornecido
- [DELETE] (api_url/jobs?name={name}&namespaceName={nsName}) - Remove o job do namespace com nome fornecido
