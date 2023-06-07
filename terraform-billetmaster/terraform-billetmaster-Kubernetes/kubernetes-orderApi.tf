#Create deployments of my orderApi microservice.
resource "kubernetes_deployment" "orderapi" {
  metadata {
    name = "orderapi"
    labels = {
      App = "orderapi"
    }
  }

  spec {
    replicas = 1 #Can be changed according to needs
    selector {
      match_labels = {
        App = "orderapi"
      }
    }
    template {
      metadata {
        labels = {
          App = "orderapi"
        }
      }
      spec {
        container {
          image = "milimolo/billetmaster:orderapi"
          name  = "orderapi"

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
resource "kubernetes_service" "orderapi" {
  metadata {
    name = "orderapi"
  }
  spec {
    selector = {
      App = kubernetes_deployment.orderapi.spec.0.template.0.metadata[0].labels.App
    }
    port {
      port        = 80
      target_port = 80
    }

    type = "LoadBalancer"
  }
}
