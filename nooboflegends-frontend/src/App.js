import React, { Component } from 'react';
import NavBar from './HomePage/NavBar';
import HomePage from './HomePage/HomePage';
import UserProfile from './UserProfile/UserProfile';
import LoadingPage from './LoadingPage/LoadingPage';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = {
            page: "Home", props: {} };
    }

    changePage = (pageId, props, caller) => {
        this.setState({ page: pageId, props: props });
    }

    render() {

        let page = [];

        switch (this.state.page) {
            case "Home":
                page.push(<HomePage key="0" changePageFunc={this.changePage} additionalProps={this.state.props} />);
                break;
            case "Loading":
                page.push(<LoadingPage key="1" changePageFunc={this.changePage} additionalProps={this.state.props} />);
                break;
            case "Profile":
                page.push(<UserProfile key="2" changePageFunc={this.changePage} additionalProps={this.state.props} />);
                break;
        }

        return (
            <div>
                <NavBar changePageFunc={this.changePage} currentPage={this.state.page} />
                {page}
            </div>
        );
    }
}
