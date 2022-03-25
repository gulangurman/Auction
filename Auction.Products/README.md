# Product Catalog Service for E-Auction on Containers

This project creates two containers:
 - a .net web api container that can be consumed by a product catalog UI
 - a mongodb container to serve data to the above product catalog microservice

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

