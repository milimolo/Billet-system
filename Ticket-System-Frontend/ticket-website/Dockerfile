# Base image
FROM node:14 as build

# Set the working directory inside the container
WORKDIR /app

# Copy package.json and package-lock.json to the working directory
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the entire project to the working directory
COPY . .

# Build the Angular application
RUN npm run build

# Stage 2: Use a smaller and efficient base image for production
FROM nginx:1.21-alpine

# Copy the built Angular app from the previous stage
COPY --from=build /app/dist/* /usr/share/nginx/html/

# Expose port 80 (default for HTTP)
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
