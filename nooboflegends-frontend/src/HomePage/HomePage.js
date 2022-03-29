import React, { Component } from 'react';
import './HomePage.css';

import banner from '../Resources/MiscIcons/nol-title-600x350.png';

/* Component that renders an entire user profile using various sub components */
export default class HomePage extends Component {

    constructor(props) {
        super(props);

        this.state = {
            changePage: props.changePageFunc, props: props.additionalProps
        };
    }

    handleSearchSubmit(event) {
        event.preventDefault();
        //Make Request to backend for user. If User is found, redirect to other page
        this.state.changePage("Profile", null);
    }


    render() {

        return (
            <div className="position-relative overflow-hidden p-3 p-md-5 m-md-3 text-center">
                <div className="col-md-5 p-lg-5 mx-auto my-5">
                    <img src={banner} />
                    <div className="form-outline">
                        <form onSubmit={this.handleSearchSubmit.bind(this)}>
                            <input type="search" id="search" className="form-control" placeholder="Search for Player" aria-label="Search" />
                        </form>
                    </div>
                </div>
            </div>
        );
    }



}