import React, { Component } from 'react';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = { message: "", loading: true };
    }

    componentDidMount() {
        this.fetchMessage();
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
            : <h2>{this.state.message}</h2>;

        return (
            <div>
                <h1>Message from the server: </h1>
                {contents}
                <p>Refresh the page to see different messages</p>
            </div>
        );
    }

    async fetchMessage() {
        const response = await fetch('helloworld');
        const data = await response.json();
        this.setState({ message: data.message, loading: false });
    }
}
