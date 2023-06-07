#Create deployments of my cacheapi microservice.
resource "kubernetes_deployment" "cacheapi" {
  metadata {
    name = "cacheapi"
    labels = {
      App = "cacheapi"
    }
  }

  spec {
    replicas = 1 #Can be changed according to needs
    selector {
      match_labels = {
        App = "cacheapi"
      }
    }
    template {
      metadata {
        labels = {
          App = "cacheapi"
        }
      }
      spec {
        container {
          image = "milimolo/billetmaster:cacheapi"
          name  = "cacheapi"

          port {
            container_port = 80
          }

          resources {
            limits = {
              cpu    = "0.5"
              memory = "512Mi"
            }
            requests = {
              cpu    = "250m"
              memory = "50Mi"
            }
          }
        }
      }
    }
  }
}



#Creates LoadBalancer for the microservice deployment above.
resource "kubernetes_service" "cacheapi" {
  metadata {
    name = "cacheapi"
  }
  spec {
    selector = {
      App = kubernetes_deployment.cacheapi.spec.0.template.0.metadata[0].labels.App
    }
    port {
      port        = 80
      target_port = 80
    }

    type = "LoadBalancer"
  }
}
