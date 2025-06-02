import type {ReactNode} from 'react';
import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';
import Link from '@docusaurus/Link';

type FeatureItem = {
  title: string;
  MyImage: string;
  href: string;
  description: ReactNode;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'Kadense Framework',
    MyImage: 'img/kadense-icon.png',
    href: '/docs/intro', 
    description: (
      <>
        Kadense simplifies harnessing the power of Kubernetes, enabling the creation of robust and highly scalable applications and services.
      </>
    ),
  },
  {
    title: 'Jupyternetes',
    MyImage: 'img/jupyternetes.png',
    href: '/docs/Products/Jupyternetes/Introduction', 
    description: (
      <>
        Jupyternetes enables running Jupyter Notebooks within a Kubernetes cluster, harnessing Kubernetes' power for data science and machine learning workflows.
      </>
    ),
  },
  {
    title: 'Consultancy',
    MyImage: 'img/consultancy.png',
    href: '/consultancy-services', 
    description: (
      <>
        Build new software, accelerate your digital transformation and optimize your IT infrastructure with our IT consultancy.
      </>
    ),
  }
];

function Feature({title, MyImage, href, description}: FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className={styles.feature}> 
        <div className="text--center">
          <img src={MyImage} className={styles.featureImage} role="img" alt={title} />
        </div>
        <div className="text--center padding-horiz--md">
          <p>{description}</p>
        </div>
        <div className="text--center padding-horiz--md">
          <Link className="button button--secondary button--lg" to={href}>
            Learn more
          </Link>
        </div>
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
