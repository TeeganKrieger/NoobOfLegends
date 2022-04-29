import React, { Component } from 'react';
import RankedHelper from '../Helpers/RankedIconHelper'
import './UserInfo.css';

/** Component that displays information about a user such as username and rank */
export default class UserInfo extends Component {

    constructor(props) {
        super(props);
        this.state = {user: props.user};
    }

    componentDidUpdate(prevProps) {
        if (this.props.user != prevProps.user) {
            this.setState({ user: this.props.user });
        }
    }

    render() {
        let user = this.state.user;

        if (user == null)
            return (<div></div>);

        let soloDuo = user.rankSoloDuo;
        let flex = user.rankFlex;
        return (
            <div className='row user-info-row'>
                <div className='col-6 col-xl-3'>
                    <p><span className='username'>{user.username}</span><span className='tagline'>#{user.tagline}</span></p>
                    <img className='rank-icon' src={RankedHelper.GetRankedIcon(soloDuo.rank)} />
                    <p className='rank-title'>Solo/Duo</p>
                    <p className='rank-title'>{RankedHelper.GetRankedName(soloDuo.rank)} {RankedHelper.GetRankedTierName(soloDuo.tier)}</p>
                    <p className='rank-title'>{soloDuo.lp} LP</p>
                </div>
                <div className='col-6 col-xl-3'>
                    <p><span className='username'>&#8205;</span></p>
                    <img className='rank-icon' src={RankedHelper.GetRankedIcon(flex.rank)} />
                    <p className='rank-title'>Flex 5v5</p>
                    <p className='rank-title'>{RankedHelper.GetRankedName(flex.rank)} {RankedHelper.GetRankedTierName(flex.tier)}</p>
                    <p className='rank-title'>{flex.lp} LP</p>
                </div>
                <div className='col-12 col-lg-6'>
                </div>
            </div>
            );
    }
}