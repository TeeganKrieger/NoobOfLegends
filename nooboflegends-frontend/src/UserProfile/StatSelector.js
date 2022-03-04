import React, { Component } from 'react';
import './StatSelector.css';

export default class StatSelector extends Component {

    constructor(props) {
        super(props);
        this.state = { stat: props.stat, lambda: props.lambda };
    }

    render() {
        let stat = this.state.stat;
        let lambda = this.state.lambda;
        return (
            <button className='stat-selector'>

            </button>
        );
    }
}