import React, { Component } from 'react';
import './NavBar.css';
import logo from '../Resources/MiscIcons/favicon-white-hat-128.png';

/* Component that renders an entire user profile using various sub components */
export default class NavBar extends Component {

    constructor(props) {
        super(props);

        this.state = {
            changePage: this.props.changePageFunc
        };
    }

    doNothing() { }

    handleReturnHome(event) {
        event.preventDefault();
        this.state.changePage("Home", {}, "NavBar");
    }

    async handleSearchSubmit(event) {
        event.preventDefault();

        let usernameAndTagline = document.getElementById("navbar-search").value;

        let search = {
            "searchFor": usernameAndTagline
        };

        this.state.changePage("Loading", search, "NavBar");
    }

    render() {

        return (
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
                <a className="navbar-brand" onClick={this.handleReturnHome.bind(this)}>
                    <img src={logo} height="40" alt="" style={{"marginLeft":"1.5rem"}}/>
                </a>

                <div className="collapse navbar-collapse" id="navbarSupportedContent">
                    <ul className="navbar-nav mr-auto">
                        <li className="nav-item active">
                            <a className="nav-link" onClick={this.handleReturnHome.bind(this)}>Home</a>
                        </li>
                        <li className="nav-item px-10">
                            <a className="nav-link" onClick={ this.doNothing() }>Compare</a>
                        </li>
                    </ul>
                    <ul className="navbar-nav" style={{ "list style": "none", "marginLeft": "auto", "marginRight": "1.5rem" }}>
                        <form className="form-inline" onSubmit={this.handleSearchSubmit.bind(this)}>
                            <div className="input-group">
                                <input id="navbar-search" className="form-control ml-sm-2" type="search" placeholder="Search for Player (Username#Tagline)" aria-label="Search" />
                                <button className="btn btn-outline-success my-2 my-sm-0" type="submit">Search</button>
                            </div>
                        </form>
                    </ul>
                </div>
            </nav >
        );
    }



}