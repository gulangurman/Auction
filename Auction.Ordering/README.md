# Ordering service

When an auction is completed, this service places an order for the product(s) on behalf of the winner, 
using the winning bid as the unit price.

Also, a message is sent via the event bus (RabbitMQ) to the auction database to inform that the auction is over.