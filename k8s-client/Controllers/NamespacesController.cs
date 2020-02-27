using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace k8s_client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NamespacesController : ControllerBase
    {
        private readonly ILogger<object> _logger;
        private KubernetesClientConfiguration kubeConfig;
        private Kubernetes kubeClient;

        public NamespacesController(ILogger<object> logger)
        {
            _logger = logger;
            kubeConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            kubeClient = new Kubernetes(kubeConfig);
        }

        [HttpGet]
        public IEnumerable<V1Namespace> Get()
        {
            var namespaces = kubeClient.ListNamespace();
            return namespaces.Items;
        }

        [HttpGet("{uid}")]
        public V1Namespace GetByUId(string uid)
        {
            var namespaces = kubeClient.ListNamespace();
            return namespaces.Items.Where(w => w.Metadata.Uid.StartsWith(uid)).SingleOrDefault();
        }

        [HttpGet("{name}/pods")]
        public IEnumerable<V1Pod> GetPods([FromRoute] string name)
        {
            var namespaces = kubeClient.ListNamespace();
            var ns = namespaces.Items.Where(w => w.Metadata.Name.StartsWith(name)).SingleOrDefault();
            return kubeClient.ListNamespacedPod(ns.Metadata.Name).Items;
        }

        [HttpPost]
        public IActionResult Post([FromQuery] string name)
        {
            var ns = new V1Namespace
            {
                Metadata = new V1ObjectMeta
                {
                    Name = name
                }
            };

            var result = kubeClient.CreateNamespace(ns);
            return Ok();
        }

        public IActionResult Delete([FromQuery] string name)
        {
            var result = kubeClient.DeleteNamespace(name);
            return Ok();
        }
    }
}
