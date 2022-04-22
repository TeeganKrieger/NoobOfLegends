import React, { Component } from 'react';
import './NavBar.css';
import logo from '../Resources/MiscIcons/favicon-white-hat-128.png';

/* Component that renders an entire user profile using various sub components */
export default class NavBar extends Component {

    constructor(props) {
        super(props);

        this.state = {
            changePage: this.props.changePageFunc,
            currentPage: this.props.currentPage
        };
    }

    componentDidUpdate(prevProps) {
        if (this.props.currentPage != prevProps.currentPage) {
            this.setState({ currentPage: this.props.currentPage });
        }
    }

    doNothing() { }

    handleReturnHome(event) {
        event.preventDefault();
        this.state.changePage("Home", {}, "NavBar");
    }

    handleComparisonPage(event) {
        event.preventDefault();
        this.state.changePage("Compare", {}, "NavBar");
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

        let searchBar = [];

        if (this.state.currentPage != "Home") {
            searchBar.push(
                <ul className="navbar-nav" style={{ "list style": "none", "marginLeft": "auto", "marginRight": "1.5rem" }}>
                    <form className="form-inline" onSubmit={this.handleSearchSubmit.bind(this)}>
                        <div className="input-group">
                            <input id="navbar-search" className="form-control" type="search" placeholder="Search for Player (Username#Tagline)" aria-label="Search" />
                            <button id="navbar-submit" className="btn" type="submit">Search</button>
                        </div>
                    </form>
                </ul>);
        }

        return (
            <nav className="navbar navbar-expand-lg">
                <a className="navbar-brand" onClick={this.handleReturnHome.bind(this)}>
                    <img src={logo} height="40" alt="" style={{ "marginLeft": "1.5rem" }} />
                </a>

                <div className="collapse navbar-collapse">

                    <ul className="navbar-nav mr-auto">
                        <li className="nav-item">
                            <a className="nav-link" onClick={this.handleReturnHome.bind(this)}>Home</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" onClick={this.handleComparisonPage.bind(this)}>Compare</a>
                        </li>
                    </ul>

                    {searchBar}

                </div>
            </nav >
        );
    }



}