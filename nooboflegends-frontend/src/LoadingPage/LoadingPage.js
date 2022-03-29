//
import React, { Component } from 'react';
import './LoadingPage.css';
import loadingGif from '../Resources/MiscIcons/ZZ5H.gif'

/* Component that renders an entire user profile using various sub components */
export default class LoadingPage extends Component {

    constructor(props) {
        super(props);

        this.state = {
            changePage: props.changePageFunc,
            searchFor: this.props.additionalProps.searchFor
        };

        console.log("About to perform search");
        this.performSearch(this.props.additionalProps.searchFor);
        console.log("Const Complete");
    }

    async performSearch(usernameAndTagline) {
        console.log("Recieved Order to do lookup for " + usernameAndTagline);
        //let usernameAndTagline = document.getElementById("navbar-search").value;
        let split = usernameAndTagline.split("#");

        if (split.length != 2) {
            const props = {
                "error": "BadRequest",
                "usernameAndTagline": usernameAndTagline
            };

            this.state.changePage("Home", props, "LoadingPage (31)");
            return;
        }

        //Make Request to backend for user. If User is found, redirect to other page
        const infoResponse = await fetch("api/info/" + split[0] + "/" + split[1], {
            method: 'GET',
            mode: 'cors',
        });
        console.log("Fetched Info");
        const matchResponse = await fetch("api/matches/" + split[0] + "/" + split[1], {
            method: 'GET',
            mode: 'cors',
        });
        console.log("Fetched Matches");

        if (infoResponse.status == 200 && matchResponse.status == 200) {
            const info = await infoResponse.json();
            const matches = await matchResponse.json();

            const props = {
                "info": info,
                "matches": matches
            };
            console.log("About to do a page change");
            this.state.changePage("Profile", props, "LoadingPage (52)");
        } else if (infoResponse.status == 400 || matchResponse.status == 400) {

            const props = {
                "error": "BadRequest",
                "usernameAndTagline": usernameAndTagline
            };

            this.state.changePage("Home", props, "LoadingPage (60)");
        } else {
            //Show Network Error
            const props = {
                "error": "Server"
            };

            this.state.changePage("Home", props, "LoadingPage (67)");
        }

    }

    render() {

        return (
            <div>
                <h1 className="loading-header">Loading Results for {this.state.searchFor}...</h1>
                <img className="loading-gif" src={loadingGif} />
                <h4 className="loading-subheader">This process might take some time.</h4>
            </div>
        );
    }



}