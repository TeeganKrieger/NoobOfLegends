import React, { Component } from 'react';
import './MatchList.css';
import MatchListing from './MatchListing'

/** Component that displays a list of matches */
export default class MatchList extends Component {

    constructor(props) {
        super(props);
        this.state = { matches: props.matches, select: props.select, deselect: props.deselect };
    }

    componentDidUpdate(prevProps) {
        if (this.props.matches != prevProps.matches) {
            this.setState({ matches: this.props.matches });
        }
    }

    render() {
        let matches = this.state.matches;
        var rows = [];

        for (let i = 0; i < matches.length; i++) {
            rows.push(<MatchListing key={i} match={this.state.matches[i]} select={this.state.select} deselect={this.state.deselect} />)
        }

        return (
            <div className='col-12 match-list'>
                {rows}
            </div>
        );
    }
}