#GKE vars
variable "gke_num_nodes" {
  default     = 1
  description = "number of gke nodes"
}

#VPC vars
variable "project_id" {
  description = "project id"
}

variable "region" {
  description = "region"
}