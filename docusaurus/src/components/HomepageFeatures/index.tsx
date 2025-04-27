import type {ReactNode} from 'react';
import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';

type FeatureItem = {
  title: string;
  MyImage: string;
  description: ReactNode;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'Kadense Framework',
    MyImage: 'img/kadense-icon.png',
    description: (
      <>
        Kadense makes it easy to unleash the power of Kubernetes to build robust, highly scalable applications and services.
      </>
    ),
  },
  {
    title: 'Jupyternetes',
    MyImage: 'img/jupyternetes.png',
    description: (
      <>
        Jupyternetes allows you to run Jupyter Notebooks in a Kubernetes cluster, enabling you to leverage the power of Kubernetes for your data science and machine learning workflows.
      </>
    ),
  }
];

function Feature({title, MyImage, description}: FeatureItem) {
  return (
    <div className={clsx('col col--6')}>
      <div className="text--center">
        <img src={MyImage} className={styles.featureImage} role="img" />
      </div>
      <div className="text--center padding-horiz--md">
        <Heading as="h3">{title}</Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): ReactNode {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
