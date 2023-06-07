#Creates an output that will display the IP address you can use to access the orderapi
output "order_lb_ip" {
  value = kubernetes_service.orderapi.status.0.load_balancer.0.ingress.0.ip
}

output "ticket_lb_ip" {
  value = kubernetes_service.ticketapi.status.0.load_balancer.0.ingress.0.ip
}

output "customer_lb_ip" {
  value = kubernetes_service.customerapi.status.0.load_balancer.0.ingress.0.ip
}

output "cache_lb_ip" {
  value = kubernetes_service.cacheapi.status.0.load_balancer.0.ingress.0.ip
}