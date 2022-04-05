document.addEventListener('DOMContentLoaded', function () {
    // connect to the auction hub
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5172/auctionhub")
        .build();

    // disable send btn until connection is established
    document.getElementById("sendButton").disabled = true;

    var auctionId = document.getElementById("AuctionId").value;
    var groupName = "auction-" + auctionId;

    connection.start()
        .then(function () {
            console.log('connection started');
            document.getElementById("sendButton").disabled = false;
            console.log('invoking AddToGroup method on hub for group: ' + groupName);
            connection.invoke("AddToGroup", groupName).catch(function (err) {
                return console.error(error.message);
            });
        }).catch(function (error) {
            return console.error(error.message);
        });
});
