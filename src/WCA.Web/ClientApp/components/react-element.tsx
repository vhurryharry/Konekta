/*
 * See:
 *  - https://blog.reyno.co.uk/aurelia-and-react/
 *  - https://ilikekillnerds.com/2015/03/how-to-use-react-js-in-aurelia/
 */

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import {
  bindable,
  autoinject,
  noView,
  Container,
  customElement,
} from 'aurelia-framework';

@autoinject
@noView
@customElement("react-element")
export class ReactComponent {

  @bindable component: string;
  @bindable props: any;

  constructor(
    private element: Element
  ) { }

  bind() {
    const component = Container.instance.get(this.component);
    const element = React.createElement(component, this.props);

    ReactDOM.render(
      element,
      this.element
    );
  }
}