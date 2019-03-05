setupConnection = () => {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("/gamehub")
        .build();

};