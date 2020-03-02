using System.Collections.Generic;
using System.Linq;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace k8s_client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly ILogger<object> _logger;
        private KubernetesClientConfiguration kubeConfig;
        private Kubernetes kubeClient;

        public ServicesController(ILogger<object> logger)
        {
            _logger = logger;
            kubeConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            kubeClient = new Kubernetes(kubeConfig);
        }

        [HttpGet]
        public IEnumerable<V1Service> Get()
        {
            var services = kubeClient.ListServiceForAllNamespaces();
            return services.Items;
        }

        [HttpGet("{uid}")]
        public V1Service GetByUId(string uid)
        {
            var services = kubeClient.ListServiceForAllNamespaces();
            return services.Items.Where(w => w.Metadata.Uid.StartsWith(uid)).SingleOrDefault();
        }

        public IActionResult Delete([FromQuery] string name, string namespaceName)
        {
            var result = kubeClient.DeleteNamespacedService(name, namespaceName);
            return Ok();
        }
    }
}
