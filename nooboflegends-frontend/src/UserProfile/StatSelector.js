import React, { Component } from 'react';
import './StatSelector.css';
import GetStatIcon from '../Helpers/StatIconHelper'

export default class StatSelector extends Component {

    constructor(props) {
        super(props);
        this.state = { stat: props.stat, lambda: props.lambda, selectStat: props.selectStat };
    }

    render() {
        let stat = this.state.stat;
        return (
            <button className='stat-selector' onClick={() => this.state.selectStat(this.state.stat, this.state.lambda)}>
                <img className='stat-icon' src={GetStatIcon(stat)} />
            </button>
        );
    }

}