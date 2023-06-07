#Create deployment of my ticketapi microservice.
resource "kubernetes_deployment" "ticketapi" {
  metadata {
    name = "ticketapi"
    labels = {
      App = "ticketapi"
    }
  }

  spec {
    replicas = 1 #Can be changed according to needs
    selector {
      match_labels = {
        App = "ticketapi"
      }
    }
    template {
      metadata {
        labels = {
          App = "ticketapi"
        }
      }
      spec {
        container {
          image = "milimolo/billetmaster:ticketapi"
          name  = "ticketapi"

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
resource "kubernetes_service" "ticketapi" {
  metadata {
    name = "ticketapi"
  }
  spec {
    selector = {
      App = kubernetes_deployment.ticketapi.spec.0.template.0.metadata[0].labels.App
    }
    port {
      port        = 80
      target_port = 80
    }

    type = "LoadBalancer"
  }
}