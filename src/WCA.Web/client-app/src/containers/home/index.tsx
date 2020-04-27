
import * as React from 'react'
import { Redirect } from 'react-router'

export default class HomePage extends React.Component {
    render(): JSX.Element {
        return (
            <Redirect to="/matter" />
        )
    }
}