# E-Auction on Containers

This project applies principles of microservices architecture to create an online auction application.

 Deploy this and call the api to view the seed product data provided by a mongodb database:
	 
	curl localhost:8000/api/v1/product

Note: 

	When overriding your connection string in a docker compose file, prefer a double underscore(__) 
	over a colon character(:) to maintain a platform-agnostic configuration.

E.g.:

Use:
 
	DatabaseSettings__ConnectionString

instead of:

	DatabaseSettings:ConnectionString


Debugging with VS
--

If you're developing with Visual studio, there is an issue with debugging.

For example, when debugging VS adds a "debug.g.yml" file to the docker-compose command:

	docker-compose -f docker-compose.yml -f docker-compose.override.yml -f "[PROJECT_PATH]\obj\Docker\docker-compose.vs.debug.g.yml" -p dockercompose14586897517941233303 --ansi never up -d --no-build

This has a drawback: when you stop debugging you cannot access the containers.
The reason for that is that it builds a specific image tagged "dev" for debugging and 
uses a different configuration which overrides your original compose files:
	 

Start your containers manually
--

However if you run your own docker-compose command:

	docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

Since the image name is not overridden, your relase image (tagged "latest") is built with this command.
You can access the containers after running this command.

To stop the containers:

	docker-compose down


Serving the API
--

We can route all requests starting with /api/ to the product catalog container.

Here's an example configuration file for serving the API through a nginx reverse proxy:

	server{
	  server_name .example.com;
	  root /var/www/example.com; 
	  location /api/ {
		client_max_body_size 2m;
		proxy_pass http://127.0.0.1:8000;
		proxy_redirect off;
		proxy_set_header Host $host;
		proxy_set_header X-Real-IP $remote_addr;
		proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
		proxy_set_header X-Forwarded-Proto $scheme;
		proxy_set_header Referer $http_referer;          
	}

