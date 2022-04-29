import React, { Component } from 'react';
import NavBar from './HomePage/NavBar';
import HomePage from './HomePage/HomePage';
import UserProfile from './UserProfile/UserProfile';
import UserComparison from './UserComparison/UserComparison';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = {
            page: "Home", props: {} };
    }

    /**
     * expression passed to pages to allow them to change to another page.
     * @param {any} pageId The id of the page to change to.
     * @param {any} props An properties to pass to the new page.
     * @param {any} caller The name of the page requesting the page change. Kept around for legacy support.
     */
    changePage = (pageId, props, caller) => {
        this.setState({ page: "", props: {} });
        this.setState({ page: pageId, props: props });
    }

    render() {

        let page = [];

        switch (this.state.page) {
            case "Home":
                page.push(<HomePage key="0" changePageFunc={this.changePage} additionalProps={this.state.props} />);
                break;
            case "Profile":
                page.push(<UserProfile key="2" changePageFunc={this.changePage} additionalProps={this.state.props} />);
                break;
            case "Compare":
                page.push(<UserComparison key="3" changePageFunc={this.changePage} additionalProps={this.state.props} />)
        }

        return (
            <div>
                <NavBar changePageFunc={this.changePage} currentPage={this.state.page} />
                {page}
            </div>
        );
    }
}
