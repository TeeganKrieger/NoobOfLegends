import React, { Component } from 'react';
import './MatchListing.css';
import GetRankedIcon from '../Helpers/RankedIconHelper'
import GetPositionIcon from '../Helpers/PositionIconHelper'
import GetChampionIcon from '../Helpers/ChampionHelper'

export default class MatchListing extends Component {

    constructor(props) {
        super(props);
        this.state = {match: props.match};
    }

    render() {
        let match = this.state.match;
        return (
            <div className='row'>
                <div className='col-5'>
                    <img className='rank-icon' src={GetRankedIcon(match.averageRank)} />
                    <img className='champion-icon' src={GetChampionIcon(match.champion)} />
                    <img className='position-icon' src='' />
                </div>
                <div className='col-3'>
                    <span className='won-span'>{match.won ? "Won" : "Lost"}</span>
                    <span className='played-on-span'>{match.playedOn}</span>
                </div>
                <div className='col-4'>
                    <span className='kda-span'>{match.kills}/{match.deaths}/{match.assists}</span>
                </div>
            </div>
            );
    }
}