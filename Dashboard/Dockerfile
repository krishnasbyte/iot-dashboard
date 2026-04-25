FROM nginx:alpine

# Copy dashboard files
COPY Dashboard/index.html /usr/share/nginx/html/
COPY Dashboard/css/ /usr/share/nginx/html/css/
COPY Dashboard/js/ /usr/share/nginx/html/js/

# Copy nginx configuration
COPY Dashboard/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost/ || exit 1
