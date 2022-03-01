import React, { Component } from 'react';
import './MatchList.css';
import MatchListing from './MatchListing'

export default class MatchList extends Component {

    constructor(props) {
        super(props);
        this.state = { matches: props.matches };
    }

    render() {
        let matches = this.state.matches;
        var rows = [];

        for (let i = 0; i < matches.length; i++) {
            rows.push(<MatchListing key={i} match={this.state.matches[i]} />)
        }

        return (
            <div className='col-12 match-list'>
                {rows}
            </div>
        );
    }
}