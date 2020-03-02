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
    public class PodsController : ControllerBase
    {
        private readonly ILogger<object> _logger;
        private KubernetesClientConfiguration kubeConfig;
        private Kubernetes kubeClient;

        public PodsController(ILogger<object> logger)
        {
            _logger = logger;
            kubeConfig = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            kubeClient = new Kubernetes(kubeConfig);
        }

        [HttpGet]
        public IEnumerable<V1Pod> Get()
        {
            var pods = kubeClient.ListPodForAllNamespaces();
            return pods.Items;
        }

        [HttpGet("{uid}")]
        public V1Pod GetByUId(string uid)
        {
            var pods = kubeClient.ListPodForAllNamespaces();
            return pods.Items.Where(w => w.Metadata.Uid.StartsWith(uid)).SingleOrDefault();
        }

        [HttpPost]
        public IActionResult Post([FromQuery] string name, string namespaceName)
        {
            var pod = new V1Pod
            {
                ApiVersion = "v1",
                Kind = "Pod",
                Metadata = new V1ObjectMeta
                {
                    Name = name
                },
                Spec = new V1PodSpec
                {
                    Containers = new List<V1Container>()
                    {
                        new V1Container()
                        {
                            Name = "container-test",
                            Image = "hello-world"                            
                        }
                    }
                }
            };

            var result = kubeClient.CreateNamespacedPod(pod, namespaceName);
            return Ok();
        }

        public IActionResult Delete([FromQuery] string name, string namespaceName)
        {
            var result = kubeClient.DeleteNamespacedPod(name, namespaceName);
            return Ok();
        }
    }
}
