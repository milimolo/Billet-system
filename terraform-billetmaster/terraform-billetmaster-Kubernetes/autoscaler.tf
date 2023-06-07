resource "kubernetes_horizontal_pod_autoscaler" "orderscaler" {
  metadata {
    name = "orderscaler"
  }

  spec {
    max_replicas = 5
    min_replicas = 2

    target_cpu_utilization_percentage = 70

    scale_target_ref {
      kind = "Deployment"
      name = "orderapi"
    }
  }
}