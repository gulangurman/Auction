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

    connection.on("Bids", function (user, bid) {
        addBidToTable(user, bid);
    })

    document.getElementById("sendButton").addEventListener("click", function (event) {
        var user = document.getElementById("SellerUserName").value;
        var productId = document.getElementById("ProductId").value;
        var sellerUser = user;
        var bid = document.getElementById("exampleInputPrice").value;

        var sendBidRequest = {
            AuctionId: auctionId,
            ProductId: productId,
            SellerUserName: sellerUser,
            Price: parseFloat(bid).toString()
        };
        SendBid(sendBidRequest);
        event.preventDefault();
    });

    document.getElementById("finishButton").addEventListener("click", function (event) { 
        var sendCompleteBidRequest = {
            AuctionId: auctionId       
        };
        SendCompleteBid(sendCompleteBidRequest);
        event.preventDefault();
    });

    function addBidToTable(user, bid) {
        var row = `<tr><td>${user}</td><td>${bid}</td></tr>`;
        if ($("table>tbody>tr:first").length > 0) {
            $("table>tbody>tr:first").before(row); // add to top
        } else {
            $('.bidLine').append(row);
        }
    }

    function SendBid(model) {
        $.ajax({
            url: "/Auction/SendBid",
            type: "POST",
            data: model,
            success: function () {               
                document.getElementById("exampleInputPrice").value = "";
                connection.invoke("SendBid", groupName, model.SellerUserName, model.Price)
                    .catch(function (err) {
                        return console.error(err.message);
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("textStatus, errorThrown");
            }
        });
    }

    function SendCompleteBid(model) {
        var id = auctionId;
        $.ajax({
            url: "/Auction/CompleteBid",
            type: "POST",
            data: { id: id },
            success: function () {                
                console.log("Bid complete successful.");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.log("textStatus, errorThrown");
            }
        });
    }
});
