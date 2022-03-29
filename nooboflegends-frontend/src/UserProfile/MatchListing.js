import React, { Component } from 'react';
import './MatchListing.css';
import GetPositionIcon from '../Helpers/PositionIconHelper'
import GetChampionIcon from '../Helpers/ChampionHelper'

/* Component that displays a single match within a match list */
export default class MatchListing extends Component {

    constructor(props) {
        super(props);
        this.state = { match: props.match, select: props.select, deselect: props.deselect };
    }

    /* Toggles selection state of the current match */
    toggleSelect() {
        let match = this.state.match;
        let select = this.state.select;
        let deselect = this.state.deselect;

        let b = document.getElementById(match.id).classList.toggle("selected");
        if (b) {
            document.getElementById(match.id).style.borderLeft = "0.5rem solid " + match.color;
            document.getElementById(match.id).style.borderRight = "0.5rem solid " + match.color;
            select(match);
        }
        else {
            document.getElementById(match.id).style.borderLeft = "";
            document.getElementById(match.id).style.borderRight = "";
            deselect(match);
        }
    }

    /* Converts a unix timestap into a date string formatted as M/D/Y H:M */
    unixTimeToDate(timestamp) {
        let date = new Date(timestamp * 1000);
        let day = date.getDate();
        let month = date.getMonth() + 1;
        let year = date.getFullYear();
        let hours = date.getHours();
        let minutes = date.getMinutes();

        return month + "/" + day + "/" + year + " " + hours + ":" + minutes;
    }

    render() {
        let match = this.state.match;

        return (
            <button id={match.id} className={"listing-box row " + (match.won ? "victory" : "defeat")} onClick={() => this.toggleSelect()}>
                <div className='col-no-padding col-4 col-md-4 col-xl-3 align-items-center justify-content-center'>
                    <img className='champion-icon' src={GetChampionIcon(match.champion)} />
                </div>
                <div className='col-no-padding col-4 col-md-5 col-xl-4'>
                    <div className={match.won ? "won-span" : "lost-span"}>{match.won ? "Victory" : "Defeat"}</div>
                    <div className='played-on-span'>{this.unixTimeToDate(match.playedOn)}</div>
                </div>
                <div className='col-no-padding col-4 col-md-3 col-xl-5'>
                    <div className='kda-title-span'>K/D/A</div>
                    <div className='kda-span'>{match.kills}/{match.deaths}/{match.assists}</div>
                </div>
            </button>
            );
    }
}