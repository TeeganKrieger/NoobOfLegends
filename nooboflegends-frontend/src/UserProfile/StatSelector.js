import React, { Component } from 'react';
import './StatSelector.css';
import GetStatIcon from '../Helpers/StatIconHelper'

/** Component that displays a single button used to select a desired stat */
export default class StatSelector extends Component {

    constructor(props) {
        super(props);
        this.state = { stat: props.stat, selectStatFunc: props.selectStatFunc };
    }

    render() {
        let stat = this.state.stat;
        return (
            <button className='stat-selector' onClick={() => this.state.selectStatFunc(stat)}>
                <img className='stat-icon' src={GetStatIcon(stat.id)} />
            </button>
        );
    }

}