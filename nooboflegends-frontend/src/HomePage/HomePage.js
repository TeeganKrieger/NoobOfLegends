import React, { Component } from 'react';
import './HomePage.css';

import banner from '../Resources/MiscIcons/nol-title-600x350.png';

/* Component that renders an entire user profile using various sub components */
export default class HomePage extends Component {

    constructor(props) {
        super(props);

        let error = props.additionalProps.error;
        let usernameAndTagline = props.additionalProps.usernameAndTagline;

        this.state = {
            changePage: props.changePageFunc, error: error, searchVal: usernameAndTagline
        };
    }

    async handleSearchSubmit(event) {
        event.preventDefault();

        let usernameAndTagline = document.getElementById("home-search").value;

        let search = {
            "searchFor": usernameAndTagline
        };

        this.state.changePage("Profile", search, "Home Page");
    }


    render() {

        let errorMsg = [];

        switch (this.state.error) {
            case "BadRequest":
                errorMsg.push(<div className="error-msg">No user with the name {this.state.searchVal} found!</div>);
                break;
            case "Server":
                errorMsg.push(<div className="error-msg">An error occured with the server!</div>);
                break;
        }

        return (
            <div className="position-relative overflow-hidden p-3 p-md-5 m-md-3 text-center">
                <div className="col-md-5 p-lg-5 mx-auto my-5">
                    <img src={banner} />
                    <div className="form-outline">
                        {errorMsg}
                        <form onSubmit={this.handleSearchSubmit.bind(this)}>
                            <input type="search" id="home-search" className="form-control" placeholder="Search for Player" aria-label="Search" />
                        </form>
                    </div>
                </div>
            </div>
        );
    }



}