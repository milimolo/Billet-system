#Create deployment of my customerApi microservice.
resource "kubernetes_deployment" "customerapi" {
  metadata {
    name = "customerapi"
    labels = {
      App = "customerapi"
    }
  }

  spec {
    replicas = 1 #Can be changed according to needs
    selector {
      match_labels = {
        App = "customerapi"
      }
    }
    template {
      metadata {
        labels = {
          App = "customerapi"
        }
      }
      spec {
        container {
          image = "milimolo/billetmaster:customerapi"
          name  = "customerapi"

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
resource "kubernetes_service" "customerapi" {
  metadata {
    name = "customerapi"
  }
  spec {
    selector = {
      App = kubernetes_deployment.customerapi.spec.0.template.0.metadata[0].labels.App
    }
    port {
      port        = 80
      target_port = 80
    }

    type = "LoadBalancer"
  }
}